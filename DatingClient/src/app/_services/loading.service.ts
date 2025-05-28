import { inject, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { delay } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class LoadingService {
  spinnerService = inject(NgxSpinnerService);
  requestCount:number = 0;

  busy()
  {
    this.requestCount++;
    this.spinnerService.show(undefined, {
       type: "line-scale-party",
       bdColor: 'rgba(255,255,255,0)',
       color:'black'
    })    
  }

  idle()
  {
    this.requestCount--;
    if(this.requestCount <= 0)
    {
      this.requestCount = 0;
      this.spinnerService.hide();
    }
  }
}
