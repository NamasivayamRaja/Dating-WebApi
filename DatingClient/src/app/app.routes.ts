import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessageComponent } from './message/message.component';
import { authGuard } from './_auth/auth.guard';
import { MemberEditComponent } from './members/member-edit/member-edit.component';
import { preventUnsavedChangesGuard } from './_guards/prevent-unsaved-changes.guard';
import { memberDetailedResolver } from './_resolvers/member-detailed.resolver';

export const routes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [authGuard],
        children: [
            { path: 'members', component: MembersListComponent },
            { path: 'members/:userName', component: MemberDetailComponent
                , resolve: { member: memberDetailedResolver} },
            { path: 'member/edit', component:MemberEditComponent, canDeactivate: [preventUnsavedChangesGuard]},
            { path: 'list', component: ListsComponent },
            { path: 'message', component: MessageComponent },
        ]
    },
    { path: '**', component: HomeComponent, pathMatch: 'full' }
];
