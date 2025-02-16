import { Component, HostListener, ViewChild } from '@angular/core';
import { MatSidenav } from '@angular/material/sidenav';

@Component({
  selector: 'app-dashboard',
  standalone: false,  
  templateUrl: './dashboard.component.html',
  styleUrl: './dashboard.component.css'
})
export class DashboardComponent {
  isScreenWide: boolean = true;
  @ViewChild(MatSidenav)
  sidenav!: MatSidenav;
  isCollapsed = true;

  constructor(

  ) {

  }

  @HostListener('window:resize', ['$event'])
  onResize(event: Event) {
    this.isScreenWide = window.innerWidth >= 768;
  }

  ngOnInit() {


    this.isScreenWide = window.innerWidth >= 768;
  }

  toggleMenu() {
    if (this.sidenav.opened) { this.sidenav.toggle(); } else { this.sidenav.open(); }

    this.isCollapsed = !this.isCollapsed;
    //this.sidenav.toggle();

  }

}
