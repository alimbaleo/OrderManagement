import { NgModule } from '@angular/core';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { FormsModule } from '@angular/forms';
import {HttpClientModule} from '@angular/common/http';
import { AuthService } from './services/auth.service';
import { SessionService } from './services/session.service';
import {authInterceptorProviders, AuthinterceptorService} from './services/authinterceptor.service';
import { AppuserService } from './services/appuser.service';

import { ReactiveFormsModule } from '@angular/forms';
import { RegisterComponent } from './components/register/register.component';
import { LoginComponent } from './components/login/login.component';
import {InputTextModule} from 'primeng/inputtext';
import {ButtonModule} from 'primeng/button'
import { OrderComponent } from './components/order/order.component';
import {TableModule} from 'primeng/table';
import { OrderService } from './services/order.service';
import {DialogModule} from 'primeng/dialog';
import {TabViewModule} from 'primeng/tabview';
import { BrowserAnimationsModule } from '@angular/platform-browser/animations';
import { UserComponent } from './components/user/user.component';

@NgModule({
  declarations: [
    AppComponent, RegisterComponent, LoginComponent, OrderComponent, UserComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    FormsModule,
    HttpClientModule,
    ReactiveFormsModule,
    InputTextModule,
    ButtonModule,
    TableModule,
    DialogModule,
    TabViewModule,
    BrowserAnimationsModule
  ],
  providers: [AuthService, SessionService, authInterceptorProviders, AppuserService, OrderService],
  bootstrap: [AppComponent]
})
export class AppModule { }
