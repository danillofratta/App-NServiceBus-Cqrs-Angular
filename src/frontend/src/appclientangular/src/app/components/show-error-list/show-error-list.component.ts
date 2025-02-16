import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-show-error-list',
  standalone: false,
  
  templateUrl: './show-error-list.component.html',
  styleUrl: './show-error-list.component.css'
})
export class ShowErrorListComponent {
  @Input() public _ListError: string[] = [];
}
