import { Routes } from '@angular/router';
import { CertificatesComponent } from './certificates/certificates.component';
import { NewCertificateComponent } from './new-certificate/new-certificate.component';

export const appRoutes: Routes = [
  { path: '', component: CertificatesComponent, pathMatch: 'full' },
  { path: 'new-certificate', component: NewCertificateComponent },
];
