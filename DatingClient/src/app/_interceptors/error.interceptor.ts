import { HttpInterceptorFn } from '@angular/common/http';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { catchError } from 'rxjs';

export const errorInterceptor: HttpInterceptorFn = (req, next) => {
  const toastservice = inject(ToastrService);

  return next(req).pipe(
    catchError(error => {
      if (error) {
        switch (error.status) {
          case 400:
            const modelStateErrors = []
            if (error.error.errors) {
              for (let err in error.error.errors) {
                if(error.error.errors[err])
                {
                  modelStateErrors.push(error.error.errors[err]);                  
                }
              }
              throw modelStateErrors.flat();
            }
            else {
              toastservice.error(error.error, error.status)
            }
            break;
          case 401:
            toastservice.error('Unauthorized', error.status);
            break;
          case 404:
            toastservice.error("Not found - Request element is not available in the server");
            break;
          case 500:
            break;
          default:
            toastservice.error("Unknown error occured. Contact support");
            break;
        }
      }
      throw error;
    })
  );
};
