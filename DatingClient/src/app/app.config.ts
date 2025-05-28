import { ApplicationConfig, importProvidersFrom } from '@angular/core';
import { provideRouter } from '@angular/router';

import { routes } from './app.routes';
import { provideHttpClient, withInterceptors } from '@angular/common/http';
import { provideAnimations } from '@angular/platform-browser/animations'
import { provideToastr } from 'ngx-toastr';
import { errorInterceptor } from './_interceptors/error.interceptor';
import { jwtInterceptor } from './_interceptors/jwt.interceptor';
import { spinningInterceptor } from './_interceptors/spinning.interceptor';
import { NgxSpinnerModule } from 'ngx-spinner';

export const appConfig: ApplicationConfig = {
  providers: [
    provideRouter(routes),
    provideHttpClient(withInterceptors([errorInterceptor,jwtInterceptor,spinningInterceptor])),
    provideAnimations(),
    provideToastr({
      positionClass: 'toast-bottom-right',
      preventDuplicates: true
    }),
    importProvidersFrom(NgxSpinnerModule)
  ]
};
