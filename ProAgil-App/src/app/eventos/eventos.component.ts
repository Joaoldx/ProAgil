import { Component, OnInit, TemplateRef } from '@angular/core';
import { ElementSchemaRegistry } from '@angular/compiler';
import { EventoService } from '../_services/evento.service';
import { Evento } from '../_models/Evento';
import { BsModalRef, BsModalService } from 'ngx-bootstrap';

@Component({
    selector: 'app-eventos',
    templateUrl: './eventos.component.html',
    styleUrls: ['./eventos.component.css']
})
export class EventosComponent implements OnInit {

    eventosFiltrados: Evento[];
    eventos: Evento[];
    imagemLargura = 50;
    imagemMargem = 2;
    mostrarImagem = false;
    _filtroLista: string;
    modalRef: BsModalRef;

    constructor(
        private eventoService: EventoService
        ,private modalService: BsModalService
    ) { }

    openModal(template: TemplateRef) {
        this.modalRef = this.modalService.show(template);
    }

    ngOnInit() {
        this.getEventos();
    }

    get filtroLista(): string {
        return this._filtroLista;
    }
    set filtroLista(value: string) {
        this._filtroLista = value;
        this.eventosFiltrados = this.filtroLista ? this.filtrarEventos(this.filtroLista) : this.eventos;
    }

    filtrarEventos(filtrarPor: string): Evento[] {
        filtrarPor = filtrarPor.toLocaleLowerCase();
        return this.eventos.filter(
            evento => evento.tema.toLocaleLowerCase().indexOf(filtrarPor) !== -1
        );
    }

    alternarImagem(){
        this.mostrarImagem = !this.mostrarImagem;
    }

    getEventos() {
        this.eventoService.getAllEvento().subscribe(
        (_eventos: Evento[]) => {
            this.eventos = _eventos;
            this.eventosFiltrados = _eventos;
            console.log(_eventos);
        }, error => {
            console.log(error);
        });
    }
}
