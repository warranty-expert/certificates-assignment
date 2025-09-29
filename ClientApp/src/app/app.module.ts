import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';
import { provideHttpClient, withInterceptorsFromDi } from '@angular/common/http';

import { AppComponent } from './app.component';
import { NavMenuComponent } from './nav-menu/nav-menu.component';
import { HomeComponent } from './home/home.component';
import { CertificatesComponent } from './certificates/certificates.component';
import { NewCertificateComponent } from './new-certificate/new-certificate.component';
import { RouterModule } from '@angular/router';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';

@NgModule({ declarations: [
        AppComponent,
        NavMenuComponent,
        HomeComponent,
        CertificatesComponent,
        NewCertificateComponent
    ],
    bootstrap: [AppComponent], imports: [FormsModule,
        ReactiveFormsModule,
        RouterModule.forRoot([
            { path: '', component: CertificatesComponent, pathMatch: 'full' },
            { path: 'new-certificate', component: NewCertificateComponent },
        ])], providers: [provideHttpClient(withInterceptorsFromDi())] })
export class AppModule { }
