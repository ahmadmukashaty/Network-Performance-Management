import { Component, Injectable, AfterViewInit, Input } from '@angular/core';
import { TREE_ACTIONS, IActionMapping, ITreeOptions, TreeModel } from 'angular-tree-component';
import {TreeNode} from 'angular-tree-component';
import { NodesService } from '../../services/getNodes.service';
import { Node } from '../../extraClasses/nodes';
import { Routes, RouterModule, Router, ActivatedRoute, Params } from '@angular/router';
import { Counter } from '../../extraClasses/counters';
import { RequestOptions, Headers, Http } from '@angular/http';
import { SubsetCounter } from '../../extraClasses/subsetCounter';
import { CounterSubset } from '../../extraClasses/counterSubset';
import { MatDialog, MatDialogConfig, MatDialogRef } from '@angular/material';
import { DialogComponent } from '../../dialog/dialog.component';
import { MessageComponent } from 'app/message/message.component';
import { ChangePath } from 'app/extraClasses/changePath';


@Component({
  selector: 'app-tree',
  templateUrl: './tree.component.html',
  styleUrls: ['./tree.component.css'],
  providers: [NodesService]
})
export class TreeComponent {
  changeCounterPath: ChangePath;
  enableMoveCounterBtn: boolean = false;
  loading: boolean;
  private nodes: Node[][] = null;
  private selectedNode: SubsetCounter;
  actionMapping: IActionMapping = {
    mouse: {
      click: (tree, node) => this.check(node)
    }
  };

  options = {
    allowDrag: true,
    displayField: 'value',
    actionMapping: this.actionMapping,
  }

  constructor(private NodeService: NodesService, private _router: Router,
     private http: Http, private route: ActivatedRoute, public dialog: MatDialog,public Messagedialog: MatDialog ) { }


  // tslint:disable-next-line:use-life-cycle-interface
  ngAfterViewInit(): void {
      this.route.params.subscribe( (params: Params) => {
      let unvName = params['unvName'];
      if (unvName === 'H6900') {
        unvName = 'H69';
      }
      this.getUniverseTree(unvName);
    });
    this.enableMoveCounterBtn = false;
  }

  public check (node: any) {
    this.selectedNode = new SubsetCounter();
    if (node.isLeaf === true) {
      this.selectedNode.value = node.data.value;
      this.selectedNode.counterID = node.data.counterID;
      this.selectedNode.tableCounterName = node.data.tableCounterName;
      this.selectedNode.tableName = node.data.tableName;
      this.selectedNode.valueType = node.data.valueType;
      this.selectedNode.path = node.data.path;
      this.selectedNode.Subset = new CounterSubset();
      this.selectedNode.Subset.SubsetID = node.data.Subset.SubsetID;
      this.selectedNode.Subset.SubsetName = node.data.Subset.SubsetName;
      this.selectedNode.Subset.Universe = node.data.Subset.Universe;
      this.selectedNode.Subset.IsActive = node.data.Subset.IsActive;
    } else {
      let parentn: string, parent1: TreeNode, nodePath: string = null;
      nodePath = node.data.value + '/';
      parent1 = node.parent;
      while ((parent1.data.value !== 'H69') && (parent1.data.value !== 'NSS') && (parent1.data.value !== 'GSN')) {
          parentn = parent1.data.value;
          nodePath = nodePath + parentn + '/';
          parent1 = parent1.parent;
      }
      this.selectedNode.path = this.InvertPath(nodePath);
    }
    this.openDialog(this.selectedNode);
  }

  getUniverseTree(universe: string) {
    this.loading = true;
    this.NodeService.getUniverseTree(universe).subscribe(values => {
      this.nodes = [values.data];
      this.loading = false;
    });

    return this.nodes;
  }

  openDialog(selectedNode: SubsetCounter) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<DialogComponent> = this.dialog.open(DialogComponent, config);
    dialogRef.componentInstance.receivedNode = selectedNode;
    dialogRef.componentInstance.enableMoveCounterBtn = this.enableMoveCounterBtn;
    dialogRef.componentInstance.changePath = this.changeCounterPath;
    dialogRef.afterClosed().subscribe(result => {
      if(result == null)
      {
        this.enableMoveCounterBtn = false;
        this.changeCounterPath = null;
      }
      else
      {
        this.changeCounterPath = (<ChangePath>result)
        if( this.changeCounterPath.actionType == "changePathAction")
        {
          this.enableMoveCounterBtn = true;
          this.OpenMessage("Please select the new path from the shown tree");
        }
        else
        {
          this.enableMoveCounterBtn = false;
          this.changeCounterPath = null;
        }
      } 
    });
  }

  OpenMessage(message: string) {
    const config = new MatDialogConfig();
    const dialogRef: MatDialogRef<MessageComponent> = this.Messagedialog.open(MessageComponent, config);
    dialogRef.componentInstance.receivedMessage = message;
  }

  InvertPath(path: string): string {
    const tokens = path.split('/');
    tokens.reverse();
    let newPath = '';
    let firstToken = true;
    for (const token of tokens) {
      if (token === '') {
        continue;
      }
      if (firstToken) {
        newPath = newPath + token;
        firstToken = false;
      } else {
        newPath = newPath + '/' + token;
      }
    }
    return newPath;
  }


  filterFn(value, treeModel: TreeModel) {
     return treeModel.filterNodes(value, true);
    // treeModel.filterNodes((node) => fuzzysearch(value, node.data.name));
  }
}


function fuzzysearch (needle, haystack) {
  const haystackLC = haystack.toLowerCase();
  const needleLC = needle.toLowerCase();

  const hlen = haystack.length;
  const nlen = needleLC.length;

  if (nlen > hlen) {
    return false;
  }
  if (nlen === hlen) {
    return needleLC === haystackLC;
  }
  outer: for (let i = 0, j = 0; i < nlen; i++) {
    const nch = needleLC.charCodeAt(i);

    while (j < hlen) {
      if (haystackLC.charCodeAt(j++) === nch) {
        continue outer;
      }
    }
    return false;
  }
  return true;
}



