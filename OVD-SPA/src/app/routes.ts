import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { AuthGuard } from './_guards/auth.guard';
import { RoleGuard } from './_guards/role.guard';
import { SessionsComponent } from './sessions/sessions.component';
import { SettingsComponent } from './settings/settings.component';
import { UserComponent } from './user/user.component';
import { NewGroupComponent } from './new-group/new-group.component';
import { EditGroupComponent } from './edit-group/edit-group.component';

export const appRoutes: Routes = [
    { path: '', component: UserComponent, canActivate: [AuthGuard] },
    { path: 'login', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [RoleGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent },
            { path: 'groups', component: GroupsComponent },
            { path: 'sessions', component: SessionsComponent },
            { path: 'settings', component: SettingsComponent },
            { path: 'new', component: NewGroupComponent },
            { path: 'edit', component: EditGroupComponent }
        ]
    },
    { path: '**', redirectTo: 'dashboard', pathMatch: 'full' }
];
