import { Component, OnDestroy, OnInit } from '@angular/core';
import { PageEvent } from '@angular/material';
import { HttpErrorResponse } from '@angular/common/http';
import { ActivatedRoute, Params } from '@angular/router';
import { Subject, Observable, BehaviorSubject } from 'rxjs';
import { takeUntil } from 'rxjs/operators';
import { MatSnackBar, MatAccordion, MatExpansionPanel, MatExpansionPanelHeader,
  MatExpansionPanelTitle, MatExpansionPanelDescription } from '@angular/material';
import { Router } from '@angular/router';

import { SearchProduct } from '../../models/search-product';
import { SearchService } from '../../services/search/search.service';
import { ProductService } from '../../services/product/product.service';
import { FormPreviewService } from '../../services/form-preview/form-preview.service';
import { FormPreviewOverlay } from '../form-product-overlay/form-preview-overlay';
import { ImagePreviewService } from '../../services/image-preview/image-preview.service';
import { ImagePreviewOverlay } from '../image-preview/image-preview-overlay';
import { AuthenticationService } from '../../modules/account/services/authentication.service';
import { environment } from '../../../environments/environment';

@Component({
  selector: 'app-search-result',
  templateUrl: './search-result.component.html',
  styleUrls: ['./search-result.component.css']
})
export class SearchresultComponent implements OnDestroy, OnInit {
  private destroy$ = new Subject<void>();
  isAuthenificated$: Observable<boolean>;
  isAuth: boolean;

  responseMessage = 'Продукт удален из Мои продукты';
  query: string;

  private tempResultSubject: BehaviorSubject<SearchProduct[]> = new BehaviorSubject<SearchProduct[]>([]);
  tempResult$: Observable<SearchProduct[]> = this.tempResultSubject.asObservable();
  searchResult: SearchProduct[];
  statusCode: number;
  tempProducts: SearchProduct[];
  errorMessage: string;
  pageSize = 5;
  pageIndex = 0;
  length = 0;

  constructor(private searchService: SearchService,
              private productService: ProductService,
              private route: ActivatedRoute,
              private previewDialogForm: FormPreviewService,
              private previewDialog: ImagePreviewService,
              public snackBar: MatSnackBar,
              public authService: AuthenticationService,
              private router: Router) {

      this.route.params.pipe(takeUntil(this.destroy$)).subscribe(({ query }: Params) => {
        if (!query) {
            return;
      }

      this.setVariablesToDefault();
      this.search(query);
      this.query = query;
    });
  }

  public ngOnDestroy(): void {
    this.destroy$.next();
    this.destroy$.complete();
  }

  public ngOnInit(): void {
    this.isAuthenificated$ = this.authService.isAuthenticated$; // MyStubs
  }

  public search(query: string) {
    this.authService.isAuthenticated$.subscribe((auth) => {
      if (auth) {
        this.searchWithAuth(query);
      } else {
        this.searchWithOutAuth(query);
      }
    });
  }

  searchWithAuth(query: string) {
    this.searchService.getDefaultAuth(query).subscribe((r) => {
      this.searchResult = r;
      if (this.searchResult) {
        this.length = this.searchResult.length;
        this.changePageEvent();
      } else {
        this.tempProducts = null;
        this.length = 0;
      }
    });
  }

  searchWithOutAuth(query: string) {
    this.searchService.getDefault(query).subscribe((r) => {
      this.searchResult = r;
      if (this.searchResult) {
        this.length = this.searchResult.length;
        this.changePageEvent();
      } else {
        this.tempProducts = null;
        this.length = 0;
      }
    });
  }

  addToMyProducts(productId: string) {
    const dialog: FormPreviewOverlay = this.previewDialogForm.open({
      form: { name: 'Добавить в Мои продукты', id: productId, searchQuery: this.query, editMode: false }
    });
  }

  async removeFromMyProducts(productId: string) {
    this.productService.deleteUserProduct(productId);
    this.snackBar.open(this.responseMessage, null, {
      duration: 3000,
      verticalPosition: 'top',
      horizontalPosition: 'center'
    });
    await this.delay(3500);
    location.reload(true);
  }

  showPreview(productName: string, picturePath: string) {
    const dialog: ImagePreviewOverlay = this.previewDialog.open({
      image: { name: productName, url: picturePath }
    });
  }

  showDetails(id: string) {
    // this.router.navigate(['', id]); // MyStubs
  }

  public changePageEvent(event?: PageEvent) {
    if (event != null) {
      this.pageIndex = event.pageIndex;
      this.pageSize = event.pageSize;
    }
    this.tempResultSubject.next(this.searchResult.slice(this.pageSize * this.pageIndex, this.pageSize * (this.pageIndex + 1)));
  return event;
  }

  private setVariablesToDefault() {
    this.pageSize = 5;
    this.pageIndex = 0;
    this.length = 0;
  }

  async delay(ms: number) {
    return new Promise( resolve => setTimeout(resolve, ms) );
  }
}
