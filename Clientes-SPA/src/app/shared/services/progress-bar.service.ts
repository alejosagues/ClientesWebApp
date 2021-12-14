import { Injectable } from '@angular/core';
import { NgProgressRef } from '@ngx-progressbar/core';

@Injectable({
  providedIn: 'root'
})
export class ProgressBarService {

  progressRef: NgProgressRef;
  default:      string = "#007bff";
  success:      string = "#4bbf73";
  error:        string = "#d9534f";
  currentColor: string = this.default;

  constructor() { }

  startLoading(){
    this.currentColor = this.default;
    this.progressRef.start();
  }

  completeLoading(){
    this.progressRef.complete();
  }

  incLoading(n: number){
    this.progressRef.inc(n);
  }

  setLoading(n: number){
    this.progressRef.set(n);
  }

  setSuccess(){
    this.currentColor = this.success;
  }

  setError(){
    this.currentColor = this.error;
  }
}
