import { Component, OnInit } from '@angular/core';
import { HttpClient } from '@angular/common/http'

@Component({
  selector: 'app-value',
  templateUrl: './value.component.html',
  styleUrls: ['./value.component.scss']
})
export class ValueComponent implements OnInit {
  clientes: any;

  constructor(private http: HttpClient) { }

  ngOnInit() {
    this.getValues();
  }

  getValues(){
    return this.http.get("http://localhost/api/clientes").subscribe(response =>{
      console.log(response);
      this.clientes = response;
    }, error => {
      console.log(error);
    });
  }
}
