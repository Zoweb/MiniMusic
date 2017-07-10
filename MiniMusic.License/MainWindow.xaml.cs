using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace MiniMusic.License
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private bool _isClosing;
        private int _stage;

        public MainWindow()
        {
            InitializeComponent();

            License.Text =
                @"MiniMusic Editor 1.0
Copyright (c) 2017 zoweb

*** END USER LICENSE AGREEMENT ***

IMPORTANT: PLEASE READ THIS LICENSE CAREFULLY BEFORE USING THIS SOFTWARE.

1. LICENSE

By receiving, opening the file package, and/or using MiniMusic Editor 1.0(""Software"") containing this software, you agree that this End User User License Agreement(EULA) is a legally binding and valid contract and agree to be bound by it. You agree to abide by the intellectual property laws and all of the terms and conditions of this Agreement.

Unless you have a different license agreement signed by zoweb your use of MiniMusic Editor 1.0 indicates your acceptance of this license agreement and warranty.

Subject to the terms of this Agreement, zoweb grants to you a limited, non-exclusive, non-transferable license, without right to sub-license, to use MiniMusic Editor 1.0 in accordance with this Agreement and any other written agreement with zoweb. zoweb does not transfer the title of MiniMusic Editor 1.0 to you; the license granted to you is not a sale. This agreement is a binding legal agreement between zoweb and the purchasers or users of MiniMusic Editor 1.0.

If you do not agree to be bound by this agreement, remove MiniMusic Editor 1.0 from your computer now and, if applicable, promptly return to zoweb by mail any copies of MiniMusic Editor 1.0 and related documentation and packaging in your possession.

2. DISTRIBUTION

MiniMusic Editor 1.0 and the license herein granted shall not be copied, shared, distributed, re-sold, offered for re-sale, transferred or sub-licensed in whole or in part except that you may make one copy for archive purposes only. For information about redistribution of MiniMusic Editor 1.0 contact zoweb.

3. USER AGREEMENT

3.1 Use

Your license to use MiniMusic Editor 1.0 is limited to the number of licenses purchased by you. You shall not allow others to use, copy or evaluate copies of MiniMusic Editor 1.0.

3.2 Use Restrictions

You shall use MiniMusic Editor 1.0 in compliance with all applicable laws and not for any unlawful purpose. Without limiting the foregoing, use, display or distribution of MiniMusic Editor 1.0 together with material that is pornographic, racist, vulgar, obscene, defamatory, libelous, abusive, promoting hatred, discriminating or displaying prejudice based on religion, ethnic heritage, race, sexual orientation or age is strictly prohibited.

Each licensed copy of MiniMusic Editor 1.0 may be used on one single computer location by one user. Use of MiniMusic Editor 1.0 means that you have loaded, installed, or run MiniMusic Editor 1.0 on a computer or similar device. If you install MiniMusic Editor 1.0 onto a multi-user platform, server or network, each and every individual user of MiniMusic Editor 1.0 must be licensed separately.

You may make one copy of MiniMusic Editor 1.0 for backup purposes, providing you only have one copy installed on one computer being used by one person. Other users may not use your copy of MiniMusic Editor 1.0 . The assignment, sublicense, networking, sale, or distribution of copies of MiniMusic Editor 1.0 are strictly forbidden without the prior written consent of zoweb. It is a violation of this agreement to assign, sell, share, loan, rent, lease, borrow, network or transfer the use of MiniMusic Editor 1.0. If any person other than yourself uses MiniMusic Editor 1.0 registered in your name, regardless of whether it is at the same time or different times, then this agreement is being violated and you are responsible for that violation!

3.3 Copyright Restriction

This Software contains copyrighted material, trade secrets and other proprietary material. You shall not, and shall not attempt to, modify, reverse engineer, disassemble or decompile MiniMusic Editor 1.0. Nor can you create any derivative works or other works that are based upon or derived from MiniMusic Editor 1.0 in whole or in part.

zoweb's name, logo and graphics file that represents MiniMusic Editor 1.0 shall not be used in any way to promote products developed with MiniMusic Editor 1.0 . zoweb retains sole and exclusive ownership of all right, title and interest in and to MiniMusic Editor 1.0 and all Intellectual Property rights relating thereto.

Copyright law and international copyright treaty provisions protect all parts of MiniMusic Editor 1.0, products and services. No program, code, part, image, audio sample, or text may be copied or used in any way by the user except as intended within the bounds of the single user program. All rights not expressly granted hereunder are reserved for zoweb.

3.4 Limitation of Responsibility

You will indemnify, hold harmless, and defend zoweb , its employees, agents and distributors against any and all claims, proceedings, demand and costs resulting from or in any way connected with your use of zoweb's Software.

In no event (including, without limitation, in the event of negligence) will zoweb , its employees, agents or distributors be liable for any consequential, incidental, indirect, special or punitive damages whatsoever (including, without limitation, damages for loss of profits, loss of use, business interruption, loss of information or data, or pecuniary loss), in connection with or arising out of or related to this Agreement, MiniMusic Editor 1.0 or the use or inability to use MiniMusic Editor 1.0 or the furnishing, performance or use of any other matters hereunder whether based upon contract, tort or any other theory including negligence.

zoweb's entire liability, without exception, is limited to the customers' reimbursement of the purchase price of the Software (maximum being the lesser of the amount paid by you and the suggested retail price as listed by zoweb ) in exchange for the return of the product, all copies, registration papers and manuals, and all materials that constitute a transfer of license from the customer back to zoweb.

3.5 Warranties

Except as expressly stated in writing, zoweb makes no representation or warranties in respect of this Software and expressly excludes all other warranties, expressed or implied, oral or written, including, without limitation, any implied warranties of merchantable quality or fitness for a particular purpose.

3.6 Governing Law

This Agreement shall be governed by the law of the Australia applicable therein. You hereby irrevocably attorn and submit to the non-exclusive jurisdiction of the courts of Australia therefrom. If any provision shall be considered unlawful, void or otherwise unenforceable, then that provision shall be deemed severable from this License and not affect the validity and enforceability of any other provisions.

3.7 Termination

Any failure to comply with the terms and conditions of this Agreement will result in automatic and immediate termination of this license. Upon termination of this license granted herein for any reason, you agree to immediately cease use of MiniMusic Editor 1.0 and destroy all copies of MiniMusic Editor 1.0 supplied under this Agreement. The financial obligations incurred by you shall survive the expiration or termination of this license.

4. DISCLAIMER OF WARRANTY

THIS SOFTWARE AND THE ACCOMPANYING FILES ARE SOLD ""AS IS"" AND WITHOUT WARRANTIES AS TO PERFORMANCE OR MERCHANTABILITY OR ANY OTHER WARRANTIES WHETHER EXPRESSED OR IMPLIED. THIS DISCLAIMER CONCERNS ALL FILES GENERATED AND EDITED BY MiniMusic Editor 1.0 AS WELL.

5. CONSENT OF USE OF DATA

You agree that zoweb may collect and use information gathered in any manner as part of the product support services provided to you, if any, related to MiniMusic Editor 1.0.zoweb may also use this information to provide notices to you which may be of use or interest to you.
";
        }

        private void YesButton_OnClick(object sender, RoutedEventArgs e)
        {
            _stage++;
            switch (_stage)
            {
                case 1:
                    License.Text = @"
The following are the licenses from the Nuget packages used in MiniMusic 1.0.
By continuing, you agree to them.

These licenses are in no particular order.

Infrangistics.Themes.MetroDark.Wpf by Brian Lagunas:

No license specified


NAudio by Mark Heath:

Licensed under the MS-PL


SharpZipLib by http://www.icsharpcode.net/

Licensed under the GPL with the following exception:

Linking this library statically or dynamically with other modules is making a combined work based on this library. Thus, the terms and conditions of the GNU General Public License cover the whole combination.

As a special exception, the copyright holders of this library give you permission to link this library with independent modules to produce an executable, regardless of the license terms of these independent modules, and to copy and distribute the resulting executable under terms of your choice, provided that you also meet, for each linked independent module, the terms and conditions of the license of that module. An independent module is a module which is not derived from or based on this library. If you modify this library, you may extend this exception to your version of the library, but you are not obligated to do so. If you do not wish to do so, delete this exception statement from your version. 

";
                    break;
                case 2:
                    File.Create(Environment.GetCommandLineArgs()[1]);
                    _isClosing = true;
                    Close();
                    break;
            }
        }

        private void NoButton_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
            Application.Current.Shutdown();
        }

        private void SplashScreen_OnClosing(object sender, CancelEventArgs e)
        {
            if (!_isClosing) e.Cancel = true;
        }
    }
}
