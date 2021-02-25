import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { DashboardModule } from './dashboard/dashboard.module';
import { ToDoListModule } from './to-do-list/to-do-list.module';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { NavBarComponent } from './nav-bar/nav-bar.component';
import { FormsModule } from '@angular/forms';

import { AuthModule } from '@auth0/auth0-angular';
import { environment as env } from '../environments/environment';

import { HTTP_INTERCEPTORS } from '@angular/common/http';
import { AuthHttpInterceptor } from '@auth0/auth0-angular';
import { AuthGuard } from '../app/auth/auth-guard';

@NgModule({
  declarations: [
    AppComponent,
    NavBarComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    DashboardModule,
    ToDoListModule,
    FormsModule,
    AuthModule.forRoot({
      ...env.auth,
      scope: 'read:to-do-lists read:to-do-list create:to-do-list update:to-do-list delete:to-do-list update-position:to-do-list ' +
             'create:to-do-item update:to-do-item delete:to-do-item share:to-do-list',
      httpInterceptor: {
        allowedList: [
          {
            uri: `${env.dev.serverUrl}/*`,
            tokenOptions: {
              audience: env.auth.audience,
              scope: 'read:to-do-lists read:to-do-list create:to-do-list update:to-do-list delete:to-do-list update-position:to-do-list create:to-do-item update:to-do-item delete:to-do-item share:to-do-list'
            }
          },
        ]
      }
    }),
  ],
  providers: [
    {
      provide: HTTP_INTERCEPTORS,
      useClass: AuthHttpInterceptor,
      multi: true,
    },
    AuthGuard
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
