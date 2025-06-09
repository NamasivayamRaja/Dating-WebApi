import { CanActivateFn } from '@angular/router';
import { AccountService } from '../_services/account-service.service';
import { ToastrService } from 'ngx-toastr';
import { inject } from '@angular/core';

export const adminGuard: CanActivateFn = (route, state) => {
  const accountService = inject(AccountService);
  const toasterService = inject(ToastrService);

  if (accountService.role().includes('Admin') || accountService.role().includes('Moderator')) {
    return true;
  }
  toasterService.error('You cannot enter this area');
  return false;
};
