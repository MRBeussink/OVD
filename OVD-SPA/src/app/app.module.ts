import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule} from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavComponent } from './nav/nav.component';
import { LoginComponent } from './login/login.component';
import { UserComponent } from './user/user.component';
import { DashboardComponent } from './dashboard/dashboard.component';
import { GroupsComponent } from './groups/groups.component';
import { SessionsComponent } from './sessions/sessions.component';
import { SettingsComponent } from './settings/settings.component';
import { RouterModule } from '@angular/router';
import { appRoutes } from './routes';
import { NavAuthService } from './nav/navauth.service';

@NgModule({
   declarations: [
      AppComponent,
      NavComponent,
      LoginComponent,
      UserComponent,
      DashboardComponent,
      GroupsComponent,
      SessionsComponent,
      SettingsComponent
   ],
   imports: [
      BrowserModule,
      FormsModule,
      RouterModule.forRoot(appRoutes),
      HttpClientModule,
      FormsModule
   ],
   providers: [
      NavAuthService
   ],
   bootstrap: [
      AppComponent
   ]
})
export class AppModule { }
