import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { BehaviorSubject } from 'rxjs';
import { Switcher, SwitcherInput } from './switcher/swticher';
import * as signalR from '@microsoft/signalr';

@Injectable({
  providedIn: 'root'
})
export class SwitcherService {
  private switchers$ = new BehaviorSubject<Switcher[]>([]);
  private hub: signalR.HubConnection;

  constructor(private http: HttpClient) {
    http.get<Switcher[]>('https://localhost:44357/switchers').subscribe(result => this.switchers$.next(result));

     this.hub = new signalR.HubConnectionBuilder().withUrl('https://localhost:44357/hub').build();
     this.hub.start().then(() => {
      this.hub.on('ProgramInputChanged', (switcherId: string, inputId: number) => this.programInputChanged(switcherId, inputId));
     });

     
  }

  private programInputChanged(switcherId: string, input: SwitcherInput) {
    const next = this.switchers$.value.map(switcher => {
      if(switcher.id === switcherId) {
        return { ...switcher, currentProgramInput : input};
      } else {
        return { ...switcher};
      }
    });

    this.switchers$.next(next);
  }

  getVisibleSwitchers() {
    return this.switchers$.asObservable();
  }

  setPreview(switcherId: string, inputId: number) {
    
  }

  setProgram(switcherId: string, inputId: number) {
    this.hub.invoke("SetProgramInput", switcherId, inputId);
  }
}
