import { Component, OnInit } from "@angular/core";

@Component({
  selector: 'app-base-page',
  template: '',
})
export abstract class BasePage  {

  protected _ListError: string[] = [];
  protected _MensageError: string = '';

}
