import { Daily } from './extraClasses/daily';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'dailypipe'
})
export class DailypipePipe implements PipeTransform {

  transform(daily: Daily[], dailysearchText: string): any[] {
    if (!daily) {
      return [];
    }
    if (!dailysearchText) {
      return daily;
    }
    dailysearchText = dailysearchText.toString().toLowerCase();
return daily.filter( it => {
      return it.SubsetID.toString().toLowerCase().includes(dailysearchText);
    });
   }

}
