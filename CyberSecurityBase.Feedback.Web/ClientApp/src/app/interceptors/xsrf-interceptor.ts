import { HttpInterceptor, HttpHandler, HttpEvent, HttpXsrfTokenExtractor, HttpRequest } from "@angular/common/http";
import { Injectable } from "@angular/core";
import { Observable } from "rxjs/Observable";

@Injectable()
export class XsrfInterceptor implements HttpInterceptor {

    constructor(private tokenExtractor: HttpXsrfTokenExtractor) { }

    private actions: string[] = ["POST", "PUT", "DELETE"];
    private forbiddenActions: string[] = ["HEAD", "OPTIONS"];

    intercept(request: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {
        let token = this.tokenExtractor.getToken();
        let permitted = this.findByActionName(request.method, this.actions);
        let forbidden = this.findByActionName(request.method, this.forbiddenActions);;

        if (permitted !== undefined && forbidden === undefined && token !== null) {
            request = request.clone({ setHeaders: { "X-XSRF-TOKEN": token } });
        }

        return next.handle(request);
    }

    private findByActionName(name: string, actions: string[]): string {
        return actions.find(action => action.toLocaleLowerCase() === name.toLocaleLowerCase());
    }
}
