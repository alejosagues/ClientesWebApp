import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';
import { AlertService } from 'ngx-alerts';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.scss']
})
export class ChangePasswordComponent implements OnInit {

  model: any = {};

  constructor(
    private route: ActivatedRoute, private authService: AuthService, public progressBar: ProgressBarService, private alertService: AlertService
    ) { }

  ngOnInit() {
    this.model.token = this.route.snapshot.queryParamMap.get('token');
    this.model.userid = this.route.snapshot.queryParamMap.get('userid');
  }

  changePassword(){
    this.alertService.info('Enviando información...');
    this.progressBar.startLoading();
    this.authService.changePassword(this.model).subscribe(() => {
      this.progressBar.setSuccess();
      console.log("Success");
      this.progressBar.completeLoading();
      this.alertService.success('Contraseña cambiada.');
    }, error => {
      this.progressBar.setError();
      console.log(error);
      this.progressBar.completeLoading();
      this.alertService.danger('No ha sido posible cambiar la contraseña.');
    });
  }

}
