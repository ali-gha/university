import { Component, OnInit } from '@angular/core';
import { LoaderService } from '../services/loader.service';

@Component({
  selector: 'app-loading-shade',
  templateUrl: './loading-shade.component.html',
  styleUrls: ['./loading-shade.component.scss']
})
export class LoadingShadeComponent implements OnInit {
  showLoader: boolean;

  constructor(private loaderService: LoaderService) { }

  ngOnInit() {
    this.loaderService.status.subscribe((re) => {
      this.showLoader = re;
    });
  }

}
