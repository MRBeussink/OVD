import { Routes } from '@angular/router';
import { LoginComponent } from './login/login.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { SessionsComponent } from './sessions/sessions.component';
import { SettingsComponent } from './settings/settings.component';


export const appRoutes: Routes = [
    { path: '', component: LoginComponent },
    { path: 'dash', component: DashboardComponent },
    { path: 'groups', component: GroupsComponent },
    { path: 'sessions', component: SessionsComponent },
    { path: 'settings', component: SettingsComponent },
    { path: '**', redirectTo: '', pathMatch: 'full' },
];
