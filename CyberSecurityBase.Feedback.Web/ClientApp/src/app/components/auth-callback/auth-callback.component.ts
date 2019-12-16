import { Component, OnInit } from '@angular/core';

import { AuthService } from '../../services/auth.service'

@Component({
  selector: 'app-auth-callback',
  templateUrl: './auth-callback.component.html',
  styles: []
})
export class AuthCallbackComponent implements OnInit {

    familyName: string;
    givenName: string;

  constructor(private authService: AuthService) { }

  ngOnInit() {
      this.authService.completeAuthentication();

      if (this.authService.isLoggedIn()) {
          var claims = this.authService.getClaims();
          this.familyName = claims.family_name;
          this.givenName = claims.given_name;
      }
  }

}
