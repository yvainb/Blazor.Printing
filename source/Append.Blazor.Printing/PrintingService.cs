﻿using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Append.Blazor.Printing
{
    public class PrintingService : IPrintingService
    {
        private JSObjectReference module;
        private readonly IJSRuntime jsRuntime;

        public PrintingService(IJSRuntime jsRuntime)
        {
            this.jsRuntime = jsRuntime;
        }

        public async Task Print(PrintOptions options)
        {
            if (module is null)
                await ImportModule();

            await module.InvokeVoidAsync("printFile", new PrintOptionsAdapter(options));
        }

        public Task Print(string printable)
        {
            return Print(new PrintOptions(printable) { Printable = printable });
        }
        public Task Print(string printable, bool showModal)
        {
            return Print(new PrintOptions(printable) { ShowModal= showModal });
        }

        internal async ValueTask ImportModule()
        {
            module = await jsRuntime.InvokeAsync<JSObjectReference>("import", "./_content/Append.Blazor.Printing/scripts.js");
        }
    }
}
