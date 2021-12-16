import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-register',
  templateUrl: './register.component.html',
  styleUrls: ['./register.component.scss']
})
export class RegisterComponent implements OnInit {

  constructor(private authService: AuthService, public progressBar: ProgressBarService, private alertService: AlertService) { }

  ngOnInit() {
  }
  onSubmit(f: NgForm) {
    this.alertService.info('Intentando registrarte...');
    this.progressBar.startLoading();

    const registerObserver = {
      next: (x:any) => {
        this.progressBar.setSuccess();
       console.log("Usuario creado");
       this.progressBar.completeLoading();
       this.alertService.success('Has sido registrado.');
      },
      error: (err:any) => {
        this.progressBar.setError();
        console.log(err);
        this.progressBar.completeLoading();
        this.alertService.danger('No ha sido posible registrarte.');
      }
    };
    this.authService.register(f.value).subscribe(registerObserver);
  }
}
