import { Component, OnInit } from '@angular/core';
import { NgForm } from '@angular/forms';
import { AuthService } from 'src/app/shared/services/auth.service';
import { ProgressBarService } from 'src/app/shared/services/progress-bar.service';

@Component({
  selector: 'app-reset-password',
  templateUrl: './reset-password.component.html',
  styleUrls: ['./reset-password.component.scss']
})
export class ResetPasswordComponent implements OnInit {

  constructor(private authService : AuthService, public progressBar: ProgressBarService) { }

  ngOnInit() {
  }
  onSubmit(f: NgForm) {
    this.progressBar.startLoading();

    const resetPasswordObserver = {
      next: (x:any) => {
        console.log("Revisa tu email para cambiar contraseña");
        this.progressBar.completeLoading();
        this.progressBar.setSuccess();
      },
      error: (err:any) => {
        this.progressBar.setError();
        console.log(err);
        this.progressBar.completeLoading();
      }
    };
    this.authService.resetPassword(f.value).subscribe(resetPasswordObserver);
  }

}
