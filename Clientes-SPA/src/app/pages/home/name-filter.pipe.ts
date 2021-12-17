import { Pipe, PipeTransform } from "@angular/core";

@Pipe({name: 'nameFilter'})
export class NameFilterPipe implements PipeTransform {
  transform(clientes, nombre) {
    return nombre ? clientes.filter(cliente => cliente.nombre === nombre) : clientes;
  }
}