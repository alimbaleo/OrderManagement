
import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders, HttpParams } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/User';
import { Order } from '../models/order';

const accountsRootUrl = 'https://localhost:7061/api/orders/'
const options = {
  headers: new HttpHeaders({ 'Content-Type': 'application/json' }),
  params: new HttpParams({})
};

@Injectable({
  providedIn: 'root'
})
export class OrderService {

  constructor(private httpClient: HttpClient) { }
getOrders(skip = 0, take = 10, email ='') : Observable<any>{
  debugger;
  options.params.append('email', email);
  options.params.append('skip', skip);
  options.params.append('take', take);
  return this.httpClient.get(accountsRootUrl + 'getList', options);

}
register(order: Order) : Observable<any>{
  debugger;
  return this.httpClient.post(accountsRootUrl + 'register', order, options);

}

}