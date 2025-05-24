import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account-service.service';
import { inject } from '@angular/core';
import { ToastrService } from 'ngx-toastr';

export const authGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toastService = inject(ToastrService);
  if(accountService.currentUser())
  {
    return true;
  }
  else{
    toastService.error("Please login");
    return false;
  }
};
