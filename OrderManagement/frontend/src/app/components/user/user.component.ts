import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import { AppuserService } from 'src/app/services/appuser.service';
import { SessionService } from 'src/app/services/session.service';
@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  users: User[] = [];
  form: User = {
    email : '',
firstName:'',
password: '',
role: '',
surname: ''
    }; 
    loading: boolean = true;
    display: boolean = false;
    hasError: boolean = false;
    errorMessage = '';
    constructor(private appUserService: AppuserService, private sessionService: SessionService) { }

  ngOnInit() {
    if (!this.sessionService.getAuthToken()) {
      window.location.replace('/login');
      return;
    }
    if(this.sessionService.getUser()?.role != 'ADMIN'){
      window.location.replace('/');
      return;
    }

    this.appUserService.getUsers().subscribe(
      data => {
        console.log(data);
        let resp = data.data;
        this.users = resp;
        this.loading = false;
      },
      err => {
        console.log(err);
        this.loading = false;
      }
    );
  }
  onSubmit(): void {
    this.appUserService.addAdminUser(this.form).subscribe(
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
  displayRegisterUserForm():void{
    debugger;
    this.display = true;
  }
  
  reloadPage(): void {
    window.location.reload();
  }
}
