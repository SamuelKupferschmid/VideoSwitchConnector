import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Switcher, SwitcherInput } from './switcher/swticher';
import { SwitcherService } from './switcher.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit {
  switchers$: Observable<Switcher[]>

  constructor(private service: SwitcherService) {  }

  ngOnInit(): void {
    this.switchers$ = this.service.getVisibleSwitchers();
  }

  public selectInput(switcher: Switcher, input: SwitcherInput) {
   this.service.setProgram(switcher.id, input.id);  
  }


}
