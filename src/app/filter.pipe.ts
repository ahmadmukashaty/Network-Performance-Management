import { Subset } from './extraClasses/subsets';
import { Pipe, PipeTransform } from '@angular/core';
@Pipe({
  name: 'filter'
})
export class FilterPipe implements PipeTransform {
  transform(subsets: Subset[], searchText: string): any[] {
    if (!subsets) {
      return [];
    }
    if (!searchText) {
      return subsets;
    }
searchText = searchText.toString().toLowerCase();
return subsets.filter( it => {
      return it.subsetID.toString().toLowerCase().includes(searchText);
    });
   }
}
