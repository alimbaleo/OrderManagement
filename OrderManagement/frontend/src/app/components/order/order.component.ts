import { Component, OnInit } from '@angular/core';
import { Order } from 'src/app/models/order';
import { OrderService } from 'src/app/services/order.service';

@Component({
  selector: 'app-order',
  templateUrl: './order.component.html',
  styleUrls: ['./order.component.css']
})
export class OrderComponent implements OnInit {
  orders: Order[] = [];
  form: Order = {
    createdBy : '',
id : 0,
orderDate : new Date(),
orderNo:'',
price: 0,
productName: '',
total: 0,
totalPrice: 0 
    }; 
  loading: boolean = true;
  display: boolean = false;
  hasError: boolean = false;
  errorMessage = '';
  constructor(private orderService: OrderService) { }

  ngOnInit() {
      this.orderService.getOrders().subscribe(
        data => {
          console.log(data);
          let resp = data.data;
          this.orders = resp;
          this.loading = false;
        },
        err => {
          console.log(err);
          this.loading = false;
        }
      );
  }

  displayOrderForm():void{
    debugger;
    this.display = true;
  }
  onSubmit(): void {
    this.orderService.register(this.form).subscribe(
      data => {
        this.hasError = false;
        console.log(data);
        let resp = data.data;
        debugger;
        this.reloadPage();
      },
      err => {
        console.log(err);
        this.hasError = true;
        this.errorMessage = err.error.errorMessage;
      }
    );
  }
  
  reloadPage(): void {
    window.location.reload();
  }
}
