import { Routes } from '@angular/router';
import { HomeComponent } from './home/home.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { AuthGuard } from './_guards/auth.guard';
import { SessionsComponent } from './sessions/sessions.component';
import { SettingsComponent } from './settings/settings.component';

export const appRoutes: Routes = [
    { path: '', component: HomeComponent },
    {
        path: '',
        runGuardsAndResolvers: 'always',
        canActivate: [AuthGuard],
        children: [
            { path: 'dashboard', component: DashboardComponent },
            { path: 'groups', component: GroupsComponent },
            { path: 'sessions', component: SessionsComponent },
            { path: 'settings', component: SettingsComponent }
        ]
    },
    { path: '**', redirectTo: 'dashboard', pathMatch: 'full' }
];
