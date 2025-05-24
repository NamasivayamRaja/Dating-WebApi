import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { MembersListComponent } from './members/members-list/members-list.component';
import { MemberDetailComponent } from './members/member-detail/member-detail.component';
import { ListsComponent } from './lists/lists.component';
import { MessageComponent } from './message/message.component';

export const routes: Routes = [
    {path: '', component: HomeComponent},
    {path: 'members', component: MembersListComponent},
    {path: 'members/:id', component: MemberDetailComponent},
    {path: 'list', component: ListsComponent},
    {path: 'message', component: MessageComponent},
    {path: '**', component: HomeComponent, pathMatch: 'full'} 
];
