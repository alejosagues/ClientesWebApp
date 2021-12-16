import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AlertService } from 'ngx-alerts';
import { Router } from '@angular/router';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {

  constructor(private authService: AuthService, public progressBar: ProgressBarService, private alertService: AlertService, private router: Router) { }

  ngOnInit() {
    if (this.authService.loggedIn()){
      this.alertService.success('Ya estás logueado.');
      this.router.navigate(['']);
    }
    }
  onSubmit(f: NgForm) {
    this.alertService.info('Intentando loguearte...');
    this.progressBar.startLoading();

    const loginObserver = {
      next: x => {
        this.progressBar.setSuccess();
        console.log("Usuario loggeado");
        this.progressBar.completeLoading();
        this.alertService.success('Has sido logueado.');
        this.router.navigate(['']);
      },
      error: err => {
        this.progressBar.setError();
        console.log(err);
        this.progressBar.completeLoading();
        this.alertService.danger("No has podido ingresar.");
      }
    };
    this.authService.login(f.value).subscribe(loginObserver);
  }
}
