import { Kpi } from './extraClasses/kpi';
import { Pipe, PipeTransform } from '@angular/core';

@Pipe({
  name: 'kpipipe'
})
export class KpipipePipe implements PipeTransform {

  transform(kpi: Kpi[], kpisearchText: string): any[] {
    if (!kpi) {
      return [];
    }
    if (!kpisearchText) {
      return kpi;
    }
    kpisearchText = kpisearchText.toString().toLowerCase();
return kpi.filter( it => {
      return it.KpiID.toString().toLowerCase().includes(kpisearchText);
    });
   }

}
