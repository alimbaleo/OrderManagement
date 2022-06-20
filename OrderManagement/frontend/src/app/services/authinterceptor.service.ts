import { Injectable } from '@angular/core';
import { HTTP_INTERCEPTORS, HttpEvent } from '@angular/common/http';
import { HttpInterceptor, HttpHandler, HttpRequest } from '@angular/common/http';
import { Observable } from 'rxjs';
import { SessionService } from './session.service';
import { Guid } from 'guid-typescript';

const KEY_FOR_AUTHORIZATION = 'Authorization';
const KEY_FOR_REQUEST_ID = 'request-id'
@Injectable({
  providedIn: 'root'
})
export class AuthinterceptorService implements HttpInterceptor {

constructor(private token: SessionService) { }
intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
  let authReq = req;
  const token = this.token.getAuthToken();
  if (token != null) {
    authReq = req.clone({ headers: req.headers.set(KEY_FOR_AUTHORIZATION, 'Bearer ' + token).set(KEY_FOR_REQUEST_ID, Guid.create().toString()) });
  }
  else{
    authReq = req.clone({headers: req.headers.append(KEY_FOR_REQUEST_ID, Guid.create().toString())})
  }
  return next.handle(authReq);
}
}

export const authInterceptorProviders = [
  { provide: HTTP_INTERCEPTORS, useClass: AuthinterceptorService, multi: true }
];
