import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { LoadingService } from '../_services/loading.service';
import { delay, finalize, identity } from 'rxjs';
import { environment } from '../../environments/environment';

export const spinningInterceptor: HttpInterceptorFn = (req, next) => {
  const loadingService = inject(LoadingService);
  loadingService.busy();
  return next(req).pipe(
    (environment.production ? identity : delay(1000)),
    finalize(() => {
      loadingService.idle();
    })
  )
};
