import { NgModule } from '@angular/core';
import { NavbarComponent } from './navbar/navbar.component';
import { SharedModule } from '../shared/shared.module';
import { NotFoundComponent } from './not-found/not-found.component';
import { LoadingShadeComponent } from './loading-shade/loading-shade.component';
import { LoaderService } from './services/loader.service';

@NgModule({
    declarations: [NavbarComponent, NotFoundComponent, LoadingShadeComponent],
    imports: [
        SharedModule
    ],
    providers: [
        LoaderService
    ],
    exports: [
        NavbarComponent,
        LoadingShadeComponent
    ]
})

export class CoreModule {

}


