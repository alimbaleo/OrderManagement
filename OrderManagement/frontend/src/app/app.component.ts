import { Component, OnInit } from '@angular/core';
import { AuthinterceptorService } from './services/authinterceptor.service';
import { SessionService } from './services/session.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent implements OnInit {
  isLoggedIn = false;
  isAdmin = false;
  username?: string;
  constructor(private sessionService: SessionService, private interceptor: AuthinterceptorService) { }
  ngOnInit(): void {
    this.isLoggedIn = !!this.sessionService.getAuthToken();
    if (this.isLoggedIn) {
      const user = this.sessionService.getUser();
      var role = user?.role;
      this.isAdmin = role == 'ADMIN';
      this.username = user?.email ?? '';
    }
  }
  logout(){
    this.sessionService.signOut();
    window.location.reload();
  }
}
