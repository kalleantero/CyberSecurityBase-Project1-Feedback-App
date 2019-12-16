import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { HttpClientModule, HTTP_INTERCEPTORS } from '@angular/common/http';
import { RouterModule } from '@angular/router';
import { AppComponent } from './app.component';
import { NavMenuComponent } from './components/nav-menu/nav-menu.component';
import { HomeComponent } from './components/home/home.component';
import { FormComponent } from './components/form/form.component';
import { FeedbacksComponent } from './components/feedbacks/feedbacks.component';
import { SafePipe } from './pipes/safepipe';
import { XsrfInterceptor } from './interceptors/xsrf-interceptor';
import { AuthCallbackComponent } from './components/auth-callback/auth-callback.component';
import { AuthService } from './services/auth.service';
import { AuthGuardService } from './services/auth-guard.service';
//import { CKEditorModule } from '@ckeditor/ckeditor5-angular';  
import { CKEditorModule } from 'ckeditor4-angular';

@NgModule({
  declarations: [
    AppComponent,
    NavMenuComponent,
    HomeComponent,
    FormComponent,
    FeedbacksComponent,
    SafePipe,
    AuthCallbackComponent    
  ],
  imports: [
    BrowserModule.withServerTransition({ appId: 'ng-cli-universal' }),
    HttpClientModule,
    FormsModule,
    CKEditorModule,
    RouterModule.forRoot([
        {
            path: '', component: HomeComponent, pathMatch: 'full'
        },
        {
            path: 'form', component: FormComponent, canActivate: [AuthGuardService]
        },
        {
            path: 'feedbacks', component: FeedbacksComponent, canActivate: [AuthGuardService]
        },
        {
            path: 'auth-callback', component: AuthCallbackComponent
        }
    ])
  ],
    providers: [
        {
            provide: HTTP_INTERCEPTORS,
            useClass: XsrfInterceptor,
            multi: true
        },
        AuthGuardService, AuthService],
  bootstrap: [AppComponent]
})
export class AppModule { }
