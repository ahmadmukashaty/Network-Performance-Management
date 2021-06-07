import { Peak } from './extraClasses/peak';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'peakfilter'
})
export class PeakfilterPipe implements PipeTransform {

  transform(peak: Peak[], peaksearchText: string): any[] {
    if (!peak) {
      return [];
    }
    if (!peaksearchText) {
      return peak;
    }
    peaksearchText = peaksearchText.toString().toLowerCase();
return peak.filter( it => {
      return it.PeakID.toString().toLowerCase().includes(peaksearchText);
    });
   }

}
