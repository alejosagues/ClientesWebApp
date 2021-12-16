import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { AlertService } from 'ngx-alerts';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';

@Component({
  selector: 'app-confirm-email',
  templateUrl: './confirm-email.component.html',
  styleUrls: ['./confirm-email.component.scss']
})
export class ConfirmEmailComponent implements OnInit {

  emailConfirmed: boolean = false;
  urlParams: any = {};

  constructor(
    private route: ActivatedRoute, private authService: AuthService, public progressBar: ProgressBarService, private router: Router, private alertService: AlertService
  ) {}

  ngOnInit() {
    this.urlParams.token = this.route.snapshot.queryParamMap.get('token');
    this.urlParams.userid = this.route.snapshot.queryParamMap.get('userid');
    this.confirmEmail();
  }

  confirmEmail(){
    this.progressBar.startLoading();
    this.authService.confirmEmail(this.urlParams).subscribe(() => {
      this.progressBar.setSuccess();
      console.log("Success");
      this.emailConfirmed = true;
      this.progressBar.completeLoading();
      this.alertService.info('Estás siendo redireccionado...');
      setTimeout(() => {
        this.router.navigate(['']);
    }, 2000);
    }, err => {
      this.progressBar.setError();
      console.log(err);
      this.progressBar.completeLoading();
      this.emailConfirmed = false;
      this.alertService.info('Estás siendo redireccionado...');
      setTimeout(() => {
        this.router.navigate(['']);
    }, 2000);
    });
  }

}
