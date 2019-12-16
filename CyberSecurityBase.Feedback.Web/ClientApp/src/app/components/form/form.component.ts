import { Component, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service'

@Component({
  selector: 'form-component',
  templateUrl: './form.component.html'
})
export class FormComponent implements OnInit  {
    httpClient: HttpClient
    familyName: string;
    givenName: string;
    email: string;

    constructor(http: HttpClient, private authService: AuthService) {
        this.httpClient = http;
    }

    ngOnInit() {

        if (this.authService.isLoggedIn()) {
            var claims = this.authService.getClaims();
            this.familyName = claims.family_name;
            this.givenName = claims.given_name;
            this.email = claims.email;
        }
    }

    public onClickSubmit(data) {

        var apiUrl = "https://localhost:44330/api/feedback";

        let headers = new HttpHeaders({
            'Authorization': this.authService.getAuthorizationHeaderValue(),
            responseType: 'text'
        })

        this.httpClient.post(apiUrl, data, { headers: headers }).subscribe(result => {
            alert("Thank you!");
        }, error => alert("Error occured. Please try again."));

    }
}
