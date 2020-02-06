import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'actionsFilter'
})
export class ActionFilterPipe implements PipeTransform {
  transform(rows: string[][], searchText1: string): any[] {
    if (!rows) {
      return [];
    }
    if (!searchText1) {
      return rows;
    }
    if(searchText1 == 'All')
    {
        return rows;
    }  
searchText1 = searchText1.toString().toLowerCase();
    var newRows:string[][];
    newRows = [];
    for (let row of rows)
    {
        if(row[0].toLowerCase()==searchText1)
        {
            newRows.push(row);
        }
    }
// return rows.filter( it => {
//       return it.counterID.toString().toLowerCase().includes(searchText1);
//     });
    return newRows;
   }
}