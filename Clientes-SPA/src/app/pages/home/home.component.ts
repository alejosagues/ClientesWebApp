import { Component, OnInit } from '@angular/core';
import * as internal from 'assert';
import { AuthService } from 'src/app/shared/services/auth.service';
import { HttpClient } from '@angular/common/http';
import { ModalDismissReasons, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormControl, ReactiveFormsModule, FormGroup, NgForm, Validators } from '@angular/forms';
import { DataTableModule } from 'angular-6-datatable';
import { filter } from 'rxjs/operators';

export interface interfazCliente {
  nombre: string;
  apellido: string;
  dni: number;
  email: string;
  usuario_creador: string;
  fecha_de_Creacion: string;
}

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.scss']
})

export class HomeComponent implements OnInit {
  
  clientes: any;
  filterData: any;
  closeResult: string;
  APIUrl = 'http://localhost:5000/api/clientes/';
  ClientForm: FormGroup;
  ClientEditForm: FormGroup;

  constructor(public authService: AuthService, private http: HttpClient, private modalService: NgbModal) { }

  ngOnInit() {
    this.getClientes();
    this.ClientForm = new FormGroup({
      nombre: new FormControl('',Validators.required),
      apellido: new FormControl('',Validators.required),
      dni: new FormControl('',Validators.required),
      email: new FormControl('',Validators.email)  
    });
    this.ClientEditForm = new FormGroup({
      nombre: new FormControl('',Validators.required),
      apellido: new FormControl('',Validators.required),
      dni: new FormControl('',Validators.required),
      email: new FormControl('',Validators.email)  
    })
  }
  
  search(term: string) {
    if(!term) {
      this.filterData = this.clientes;
    } else {
      this.filterData = [... new Set([ ...this.clientes.filter(x => 
         x.nombre.trim().toLowerCase().includes(term.trim().toLowerCase())
      ), ...
      this.clientes.filter(x => 
        x.apellido.trim().toLowerCase().includes(term.trim().toLowerCase())
      ), ...
      this.clientes.filter(x => 
        x.email.trim().toLowerCase().includes(term.trim().toLowerCase())
      ), ...
      this.clientes.filter(x => 
        x.fecha_de_Creacion.trim().toLowerCase().includes(term.trim().toLowerCase())
      ), ...
      this.clientes.filter(x => 
        x.dni.toString().trim().toLowerCase().includes(term.trim().toLowerCase())
      )
      ])];
    }
  }

  onSubmit_crear(f) {
    f.value.usuario_creador = this.authService.decodedToken.unique_name
    this.http.post(this.APIUrl + 'crear', f.value)
      .subscribe((result) => {
        this.ngOnInit();
      });
    this.modalService.dismissAll();
  }

  onSubmit_editar(f, clientes) {
    console.log(f.value);
    f.value.usuario_creador = this.authService.decodedToken.unique_name
    f.value.id = clientes.id
    this.http.put(this.APIUrl + f.value.id , f.value)
      .subscribe((result) => {
        this.ngOnInit();
      });
    this.modalService.dismissAll();
    location.reload();
  }

  getClientes(){
    return this.http.get(this.APIUrl).subscribe(response =>{
      this.clientes = response;
      this.filterData = response;
      console.log(response);
    }, error => {
      console.log(error);
    });
  }

  deleteClientes(id: number){
    return this.http.delete(this.APIUrl + id).subscribe(response =>{
      console.log(response);
      this.clientes = response;
      location.reload();
    }, error => {
      console.log(error);
    });
  }

  open(content: any){
    this.modalService.open(content, {ariaLabelledBy: 'modal-basic-title'}).result.then(result => {
      this.closeResult = 'Closed with: $(result)';
    }, reason => {
      this.closeResult = 'Dismissed $(this.getDismissReason(reason))';
    });
  }

  openEditar(targetModal, clientes) {
    this.modalService.open(targetModal, {
     centered: true,
     backdrop: 'static',
     size: 'lg'
   });
    this.ClientEditForm = new FormGroup({
      nombre: new FormControl(clientes.nombre,Validators.required),
      apellido: new FormControl(clientes.apellido,Validators.required),
      dni: new FormControl(clientes.dni,Validators.required),
      email: new FormControl(clientes.email,Validators.email)  
    })
    document.getElementById('nombre.e').setAttribute('value', clientes.nombre);
    document.getElementById('apellido.e').setAttribute('value', clientes.apellido);
    document.getElementById('dni.e').setAttribute('value', clientes.dni);
    document.getElementById('email.e').setAttribute('value', clientes.email);
 }

  private getDismissReason(reason: any): string {
    if (reason === ModalDismissReasons.ESC) {
      return 'by pressing ESC';
    } else if (reason === ModalDismissReasons.BACKDROP_CLICK) {
      return 'by clicking on a backdrop';
    } else {
      return `with: ${reason}`;
    }
  }
}
