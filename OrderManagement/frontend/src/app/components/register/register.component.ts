import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/models/User';
import { AuthService } from 'src/app/services/auth.service';
import { AbstractControl, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { SessionService } from 'src/app/services/session.service';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.css']
})
export class RegisterComponent implements OnInit {

  form: User = {
    firstName: null,
    surname: null,
    email: null,
    password: null,
    role: null
  };
  isSuccessful = false;
  errorMessage = '';
  isSignUpFailed = false;
  constructor(private authservice: AuthService, private sessionService: SessionService) { }

  ngOnInit() {
    if (this.sessionService.getAuthToken()) {
      window.location.replace('/');
    }
  }
  onSubmit(): void {
    this.authservice.register(this.form).subscribe(
      data => {
        console.log(data);
        this.isSuccessful = true;
        let resp = data.data;
        this.sessionService.saveAuthToken(resp.token, resp.expiration);
        this.sessionService.saveUser(resp);
        window.location.replace('/');
      },
      err => {
        this.errorMessage = err.error.errorMessage;
        this.isSuccessful = false;
        this.isSignUpFailed = true;
      }
    );
  }
}
