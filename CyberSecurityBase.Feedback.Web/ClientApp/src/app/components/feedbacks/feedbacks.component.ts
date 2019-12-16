import { Component, Inject, OnInit } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'feedbacks-component',
  templateUrl: './feedbacks.component.html'
})
export class FeedbacksComponent implements OnInit {
    public feedbacks: Feedback[];
    httpClient: HttpClient
    isUser: boolean;

    constructor(http: HttpClient, private authService: AuthService, @Inject('BASE_URL') baseUrl: string) {
        this.httpClient = http;
        this.getFeedbacks();
    }

    ngOnInit() {
        this.isUser = true;
        if (this.authService.isLoggedIn()) {
            var role = this.getRole();
            if (role === "Admin") {
                this.isUser = false;
            }
        }
    }

    getFeedbacks() {
        var apiUrl = "https://localhost:44330/api/feedback";

        let headers = new HttpHeaders({
            'Authorization': this.authService.getAuthorizationHeaderValue(),
            responseType: 'text'
        })

        var role = this.getRole();

        if (role === "Admin") {
            apiUrl = "https://localhost:44330/api/feedback/admin";
        }

        this.httpClient.get<Feedback[]>(apiUrl, { headers: headers }).subscribe(result => {
            this.feedbacks = result;
        }, error => console.error(error));
    }

    getRole() {
        if (this.authService.isLoggedIn()) {
            var claims = this.authService.getClaims();
            if (claims) {
                return claims.role;
            }
        }
    }

    public onClickSubmit(data) {

        var apiUrl = "https://localhost:44330/api/feedback/search?keywords="+data.search;

        let headers = new HttpHeaders({
            'Authorization': this.authService.getAuthorizationHeaderValue(),
            responseType: 'text'
        })

        this.httpClient.get<Feedback[]>(apiUrl, { headers: headers }).subscribe(result => {
            this.feedbacks = result;
        }, error => console.error(error));

    }
}
