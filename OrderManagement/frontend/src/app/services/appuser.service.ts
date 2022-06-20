import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';

import { Observable } from 'rxjs';
import { User } from '../models/User';

const options = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' })
};
const userBaseUrl = 'https://localhost:7061/api/appusers/';
const accountBaseUrl = 'https://localhost:7061/api/accounts/';
@Injectable({
  providedIn: 'root'
})
export class AppuserService {

constructor(private httpClient: HttpClient) { }
getUsers(): Observable<any> {
  return this.httpClient.get(userBaseUrl + 'getlist', { responseType: 'json' });
}

register(user: User) : Observable<any>{
  return this.httpClient.post(userBaseUrl + 'register', user, options);

}
addAdminUser(user: User) : Observable<any>{
  return this.httpClient.post(accountBaseUrl + 'addAdminUser', user, options);

}
}
