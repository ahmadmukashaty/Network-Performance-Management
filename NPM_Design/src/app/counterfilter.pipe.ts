
import { Pipe, PipeTransform } from '@angular/core';
import { Counter } from 'app/extraClasses/counters';
@Pipe({
  name: 'counterfilter'
})
export class CounterFilterPipe implements PipeTransform {
  transform(counters: Counter[], searchText1: string): any[] {
    if (!counters) {
      return [];
    }
    if (!searchText1) {
      return counters;
    }
searchText1 = searchText1.toString().toLowerCase();
return counters.filter( it => {
      return it.counterID.toString().toLowerCase().includes(searchText1);
    });
   }
}
